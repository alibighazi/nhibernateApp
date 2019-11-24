namespace NhibernateApp.Models
{
    public class AuditLogDetail
    {
        public virtual string Id { get; set; }
        public virtual AuditLog AuditLog { get; set; }
        public virtual string FieldName { get; set; }
        public virtual string OldValue { get; set; }
        public virtual string NewValue { get; set; }

    }
}