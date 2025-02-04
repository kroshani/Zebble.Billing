﻿namespace Zebble.Billing
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    class AppSubscriptionStatusMiddleware
    {
        public AppSubscriptionStatusMiddleware(RequestDelegate _) { }

        public async Task InvokeAsync(HttpContext context, ISubscriptionManager manager)
        {
            var model = await context.Request.Body.ConvertTo<AppSubscriptionStatusModel>();

            var status = await manager.GetSubscriptionStatus(model.UserId);

            await context.Response.WriteAsync(status.ToJson());
        }
    }

    class AppSubscriptionStatusModel
    {
        public string Ticket { get; set; }
        public string UserId { get; set; }
    }
}
