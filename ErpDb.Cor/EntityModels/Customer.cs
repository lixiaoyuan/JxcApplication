using System.ComponentModel;
using BusinessDb.Cor.Models;

namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            CustomerAccountRecord = new HashSet<CustomerAccountRecord>();
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Guid? AcontactId { get; set; }

        public string AcontactName { get; set; }

        public string AcontactTel { get; set; }
        /// <summary>
        /// ����ҵ��Ա
        /// </summary>
        public Guid? ResponsibleSalesman { get; set; }
        public string Tel { get; set; }

        public string Fax { get; set; }
        /// <summary>
        /// �ͻ�����
        /// </summary>
        public CustomerType CustomerType { get; set; }
        /// <summary>
        /// �ͻ���������
        /// </summary>
        public PaymentType PaymentType { get; set; }


        public string Area { get; set; }

        public string Address { get; set; }

        public decimal? Credibility { get; set; }

        public decimal? Balance { get; set; }

        public bool? Enable { get; set; }

        public virtual Acontact Acontact { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerAccountRecord> CustomerAccountRecord { get; set; }
    }
}
