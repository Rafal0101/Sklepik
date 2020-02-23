using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.States
{
    public enum OrderStatus
    {
        Submitted,
        InReview,
        Accepted,
        Rejected
    }
}
