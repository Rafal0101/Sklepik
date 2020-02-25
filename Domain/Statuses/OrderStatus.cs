using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.States
{
    public enum OrderStatus
    {
        All,
        Submitted,
        InReview,
        Accepted,
        Rejected
    }
}
