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
    : DropCreateDatabaseAlways<SupportCenterDbContext>
  {
      protected override void Seed(SupportCenterDbContext context)
    {
      
       TicketRepositoryHC trt = new TicketRepositoryHC();
        context.Tickets.AddRange(trt.ReadTickets());

      // Save the changes in the context (all added entities) to the database
      context.SaveChanges();
    }
  }
}
