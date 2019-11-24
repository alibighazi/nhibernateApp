using System;
using System.Collections.Generic;

namespace NhibernateApp.Models
{
    public class AuditLog
    {
        public virtual string Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Action { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual string TableName { get; set; }
        public virtual string RecordId { get; set; }
        public virtual IList<AuditLogDetail> AuditLogDetails { get; set; }
    }
}
