using System;
using Microsoft.OpenApi.Any;

namespace Locum_Backend.Models
{

    public class RequestData
    {
        public Patient? patient { get; set; }
        public ApprovalRequest? approvalRequest { get; set; }

    }
}