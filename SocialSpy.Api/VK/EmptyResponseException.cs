using System;

namespace SocialSpy.Api.VK
{
    public class EmptyResponseException:Exception
    {
        public override string Message
        {
            get { return "Empty response was received"; }
        }
    }
}