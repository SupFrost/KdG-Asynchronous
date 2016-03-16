using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SC.BL;
using SC.BL.Domain;
using SC.UI.CA.ExtensionMethods;
using static System.Console;

namespace SC.UI.CA
{
    internal class Program
    {
        private static bool quit;
        private static ITicketManager mgr;
        private static Service srv;

        private static void Main(string[] args)
        {
            mgr = new TicketManager();
            srv = new Service();


            while (!quit)
                ShowMenu();
        }

        private static void ShowMenu()
        {
            WriteLine("=================================");
            WriteLine("=== HELPDESK - SUPPORT CENTER ===");
            WriteLine("=================================");
            WriteLine("1) Toon alle tickets");
            WriteLine("2) Toon details van een ticket");
            WriteLine("3) Toon de antwoorden van een ticket");
            WriteLine("4) Maak een nieuw ticket");
            WriteLine("5) Geef een antwoord op een ticket");
            WriteLine("6) Markeer ticket als 'Closed'");
            WriteLine("0) Afsluiten");
            try
            {
                DetectMenuAction();
            }
            catch (Exception)
            {
                WriteLine();
                WriteLine("Er heeft zich een onverwachte fout voorgedaan!");
                WriteLine();
            }
        }

        private static void DetectMenuAction()
        {
            var inValidAction = false;
            do
            {
                Write("Keuze: ");
                var input = ReadLine();
                int action;
                if (int.TryParse(input, out action))
                {
                    switch (action)
                    {
                        case 1:
                            PrintAllTickets();
                            break;
                        case 2:
                            ActionShowTicketDetails();
                            break;
                        case 3:
                            ActionShowTicketResponses();
                            break;
                        case 4:
                            ActionCreateTicket();
                            break;
                        case 5:
                            ActionAddResponseToTicket();
                            break;
                        case 6:
                            ActionCloseTicket();
                            break;
                        case 0:
                            quit = true;
                            return;
                        default:
                            WriteLine("Geen geldige keuze!");
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
        }

        private static void ActionCloseTicket()
        {
            Write("Ticketnummer: ");
            var input = int.Parse(ReadLine());

            //mgr.ChangeTicketStateToClosed(input);
            // via WebAPI-service
            srv.ChangeTicketStateToClosed(input);
        }

        private static void PrintAllTickets()
        {
            Parallel.ForEach(mgr.GetTickets(), b => WriteLine(b.GetInfo()));

          
              
        }

        private static void ActionShowTicketDetails()
        {
            Write("Ticketnummer: ");
            var input = int.Parse(ReadLine());

            Ticket t = mgr.GetTicket(input);
            PrintTicketDetails(t);
        }

        private static void PrintTicketDetails(Ticket ticket)
        {
            WriteLine("{0,-15}: {1}", "Ticket", ticket.TicketNumber);
            WriteLine("{0,-15}: {1}", "Gebruiker", ticket.AccountId);
            WriteLine("{0,-15}: {1}", "Datum", ticket.DateOpened.ToString("dd/MM/yyyy"));
            WriteLine("{0,-15}: {1}", "Status", ticket.State);

            if (ticket is HardwareTicket)
                WriteLine("{0,-15}: {1}", "Toestel", ((HardwareTicket) ticket).DeviceName);

            WriteLine("{0,-15}: {1}", "Vraag/probleem", ticket.Text);
        }

        private static void ActionShowTicketResponses()
        {
            Write("Ticketnummer: ");
            var input = int.Parse(ReadLine());

            //IEnumerable<TicketResponse> responses = mgr.GetTicketResponses(input);
            // via Web API-service
            var responses = srv.GetTicketResponsesAsync(input);
            if (responses != null) PrintTicketResponses(responses.Result);
        }

        private static void PrintTicketResponses(IEnumerable<TicketResponse> responses)
        {
            Parallel.ForEach(responses, r => WriteLine());


        }

        private static void ActionCreateTicket()
        {
            var accountNumber = 0;
            var problem = "";
            var device = "";

            Write("Is het een hardware probleem (j/n)? ");
            var isHardwareProblem = ReadLine().ToLower() == "j";
            if (isHardwareProblem)
            {
                Write("Naam van het toestel: ");
                device = ReadLine();
            }

            Write("Gebruikersnummer: ");
            accountNumber = int.Parse(ReadLine());
            Write("Probleem: ");
            problem = ReadLine();

            if (!isHardwareProblem)
                mgr.AddTicket(accountNumber, problem);
            else
                mgr.AddTicket(accountNumber, device, problem);
        }

        private static void ActionAddResponseToTicket()
        {
            Write("Ticketnummer: ");
            var ticketNumber = int.Parse(ReadLine());
            Write("Antwoord: ");
            var response = ReadLine();

            //mgr.AddTicketResponse(ticketNumber, response, false);
            // via WebAPI-service
            srv.AddTicketResponse(ticketNumber, response, false);
        }
    }
}