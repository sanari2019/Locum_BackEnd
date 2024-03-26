using System;
using Microsoft.OpenApi.Any;

namespace Locum_Backend.Models
{

    public class RequestData
    {
        public NursedPatient? patient { get; set; }
        public ApprovalRequest? approvalRequest { get; set; }

    }
}