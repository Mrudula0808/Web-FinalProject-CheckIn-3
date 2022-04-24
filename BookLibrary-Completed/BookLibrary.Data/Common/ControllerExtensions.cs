using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Data.Common
{
    public static class ControllerExtensions
    {
        public static void AddMessage(this Controller controller, string? message)
        {
            message ??= "Undefined error";
            controller.ViewData["message"] = message;
        }
    }
}
