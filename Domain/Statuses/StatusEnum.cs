using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Statuses
{
    public enum StatusEnum
    {
        Submitted,
        InReview,
        Rejected,
        PartiallyAccepted,
        Accepted,
        Paid,
        Delivered,
        Historical
    }
}
