using BookStore.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace BookStore.Services
{
    public interface ISmsService
    {
        Task<MessageResource> Send(SmsMessage smsMessage);
    }
}
