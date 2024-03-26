using System;
using RepoDb.Attributes;

namespace Locum_Backend.Models
{


    public class ForgotPasswordDto
    {

        public string? Email { get; set; }


        public string? ClientURI { get; set; }
    }

}