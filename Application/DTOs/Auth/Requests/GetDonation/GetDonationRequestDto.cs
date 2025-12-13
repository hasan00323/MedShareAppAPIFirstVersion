using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Requests.GetDonation
{
    public class GetDonationRequestDto
    {
        public int UserId { get; set; }
        public int DonationId { get; set; }
    }
}
