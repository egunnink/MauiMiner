using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MauiMiner
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Offering OfferingEnum { get; set; }

        public static Session FromToken(JToken tok)
        {
            Offering flag = Offering.NONE;
            flag = flag.FromString((string)tok["shortDescription"]);
            return new Session()
            {
                Id = (int)tok["id"],
                StartDate = DateTime.Parse((string)tok["startDate"]),
                EndDate = DateTime.Parse((string)tok["endDate"]),
                OfferingEnum = flag
            };
        }

        public static Dictionary<Offering, Session> GetPreviousYearsSessions()
        {
            Session current = Session.FromToken(MauiAPI.GetCurrentSession());
            Dictionary<Offering, Session> sessions = new Dictionary<Offering, Session>();
            List<JToken> tokens = MauiAPI.GetSessionRange(current.Id, -3);
            foreach(JToken tok in tokens)
            {
                Session session = Session.FromToken(tok);
                sessions.Add(session.OfferingEnum, session);
            }
            return sessions;
        }
    }
}
