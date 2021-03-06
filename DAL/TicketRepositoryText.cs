﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SC.BL.Domain;
namespace SC.DAL
{
  public class TicketRepositoryText : ITicketRepository
  {
    private readonly List<TicketResponse> responses;
    private readonly List<Ticket> tickets;
    public TicketRepositoryText()
    {
      tickets = new List<Ticket>();
      responses = new List<TicketResponse>();
      SeedAsync();
    }
    public Ticket CreateTicket(Ticket ticket)
    {
      ticket.TicketNumber = tickets.Count + 1;
      tickets.Add(ticket);
      return ticket;
    }

      public Task<Ticket> CreateTicketAsync(Ticket ticket)
      {
          throw new NotImplementedException();
      }

      public IEnumerable<Ticket> ReadTickets()
    {
      return tickets;
    }
    public Ticket ReadTicket(int ticketNumber)
    {
      return tickets.Find(t => t.TicketNumber == ticketNumber);
    }
    public void UpdateTicket(Ticket ticket)
    {
      // Do nothing! All data lives in memory, everything references the same objects!!
    }
    public void DeleteTicket(int ticketNumber)
    {
      responses.RemoveAll(r => r.Ticket.TicketNumber == ticketNumber);
      tickets.Remove(ReadTicket(ticketNumber));
    }
    public IEnumerable<TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber)
    {
      return tickets.Find(t => t.TicketNumber == ticketNumber).Responses;
    }
    public TicketResponse CreateTicketResponse(TicketResponse response)
    {
      response.Id = responses.Count + 1;
      responses.Add(response);
      return response;
    }
    public void UpdateTicketStateToClosed(int ticketNumber)
    {
      // Do nothing! All data lives in memory, everything references the same objects!!
    }
    private async void SeedAsync()
    {
      Task<List<Ticket>> ticketss = DeserializeAsync();
      Console.WriteLine("Test");
      this.tickets.AddRange(await ticketss);
    }
    private static async Task<List<Ticket>> DeserializeAsync()
    {
      using (StreamReader stream = new StreamReader("tickets.xml"))
      {
        var xml = await stream.ReadToEndAsync();
        XmlReader reader = XmlReader.Create(new StringReader(xml));
        XmlSerializer serializer = new XmlSerializer(typeof(List<Ticket>));
        return (List<Ticket>)serializer.Deserialize(reader);
      }

    }
    public async void SerializeAsync(List<Ticket> tickets)
    {
      using (StreamWriter stream = new StreamWriter("tickets.xml"))
      {
        XmlSerializer ser = new XmlSerializer(typeof(List<Ticket>));
        StringWriter writer = new StringWriter();
        ser.Serialize(writer, tickets);
        await stream.WriteAsync(writer.ToString());
      }
    }
  }
}