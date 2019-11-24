using System;

namespace NhibernateApp.Models
{
    public class AuditableBaseModel<T> : BaseModel<T>
    {

        private string _createdBy;
        private DateTime _createdOn;
        private string _updatedBy;
        private DateTime _updatedOn;


        public virtual string CreatedBy
        {
            get
            {
                if (!string.IsNullOrEmpty(_createdBy)) return _createdBy;
                return "galibi111";
            }
            set { _createdBy = value; }
        }
        public virtual DateTime CreatedOn
        {
            get { return DateTime.Now; }
            set { _createdOn = value; }
        }
        public virtual string UpdatedBy
        {
            get { return "galibi"; }
            set { _updatedBy = value; }
        }
        public virtual DateTime UpdatedOn
        {
            get { return DateTime.Now; }
            set { _updatedOn = value; }
        }

    }
}
