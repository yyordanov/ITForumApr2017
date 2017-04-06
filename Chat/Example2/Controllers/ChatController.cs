using Example2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Example2.Controllers
{
    public class ChatController : ApiController
    {
        // GET: api/Chat
        public IEnumerable<ChatMessage> Get()
        {
            return messages;
        }

        // GET: api/Chat/5
        public HttpResponseMessage Get(int id)
        {
            var message = messages.Where(x => x.Id == id).SingleOrDefault();
            if (message == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Chat message with the ID={id} doesn't exist");
            }

            return Request.CreateResponse(message);
        }

        // POST: api/Chat
        public HttpResponseMessage Post([FromBody]ChatMessage value)
        {
            value.Id = messages.Max(x => x.Id) + 1;
            var name = User?.Identity?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                value.UserName = name;
            }
            value.Timestamp = DateTime.Now;

            messages.Add(value);

            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        // PUT: api/Chat/5
        public HttpResponseMessage Put(int id, [FromBody]ChatMessage value)
        {
            var toUpdate = messages.Where(x => x.Id == id).SingleOrDefault();
            if (toUpdate == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Chat message with the ID={id} doesn't exist");
            }

            toUpdate.Message = value.Message;
            value.Timestamp = DateTime.Now;

            return Request.CreateResponse(HttpStatusCode.OK, value);
        }

        // DELETE: api/Chat/5
        public HttpResponseMessage Delete(int id)
        {
            var toDelete = messages.Where(x => x.Id == id).SingleOrDefault();
            if (toDelete == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Chat message with the ID={id} doesn't exist");
            }

            messages.Remove(toDelete);

            return Request.CreateResponse(HttpStatusCode.OK);
        }



        static ChatController()
        {
            messages = new List<ChatMessage>();
            messages.Add(new ChatMessage
            {
                Id = 1,
                UserName = "kiro",
                Message = "Здравей",
                Timestamp = DateTime.Now.AddMinutes(-5)
            });
            messages.Add(new ChatMessage
            {
                Id = 2,
                UserName = "dimo",
                Message = "Опа :)",
                Timestamp = DateTime.Now.AddMinutes(-1)
            });
        }

        private static readonly List<ChatMessage> messages;
    }
}
