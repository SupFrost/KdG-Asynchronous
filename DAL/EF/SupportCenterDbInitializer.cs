using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SC.BL.Domain;
namespace SC.DAL.EF
{
  internal class SupportCenterDbInitializer 
    : DropCreateDatabaseIfModelChanges<SupportCenterDbContext>
  {
    protected override void Seed(SupportCenterDbContext context)
    {
      
            
     TicketRepositoryText trt = new TicketRepositoryText();
        context.Tickets.AddRange(trt.ReadTickets());




            


      // Save the changes in the context (all added entities) to the database
      context.SaveChanges();
    }
  }
}
