using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediService.ASP.NET_Core.Services.Appointments;
using MediService.ASP.NET_Core.Models.Messages;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Services.Specialists;
using MediService.ASP.NET_Core.Services.Messages;

using static MediService.ASP.NET_Core.WebConstants.GlobalMessage;

namespace MediService.ASP.NET_Core.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IAppointmentService appointments;
        private readonly ISpecialistService specialists;
        private readonly IMessageService messages;

        public MessagesController(
            IAppointmentService appointments,
            ISpecialistService specialists,
            IMessageService messages)
        {
            this.appointments = appointments;
            this.specialists = specialists;
            this.messages = messages;
        }

        public IActionResult Send(string id)
        {
            var isValid = this.appointments.IsValid(id);
            if (!isValid)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(string id, MessageFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var appointment = this.appointments.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            var userId = this.User.Id();
            string recipientId;
            string sender;
            var isSpecialist = this.specialists.IsSpecialist(userId);
            if (isSpecialist)
            {
                recipientId = appointment.UserId;
                sender = appointment.SpecialistFullName;
            }
            else
            {
                recipientId = appointment.SpecialistUserId;
                sender = appointment.UserFullName;
            }
            await this.messages.Send(recipientId, sender, model.Title, model.Content);

            TempData.Add(SuccessKey, "Message sent.");
            return Redirect("/Appointments/Mine");
        }

        public int GetCount()
        {
            var newMessages = this.messages.GetCountNew(this.User.Id());

            return newMessages;
        }
    }
}
