using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemUser()
        {
            AuthUserRole = new HashSet<AuthUserRole>();
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// ƴ��
        /// </summary>
        public string Spell { get; set; }
        /// <summary>
        /// �Ա�
        /// </summary>
        public bool? Gender { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// �绰
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// �ʼ�
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ���֤��
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// ����֤����
        /// </summary>
        public DateTime? HealthCertificateExpired { get; set; }
        /// <summary>
        /// ѧ��
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// ѧУ
        /// </summary>
        public string School { get; set; }
        /// <summary>
        /// ��ַ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public bool? Enable { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public bool? RemindBirthday { get; set; }
        /// <summary>
        /// ����֤��������
        /// </summary>
        public bool? RemindHealthCertificateExpired { get; set; }
        public bool? Check { get; set; }

        public DateTime? ChangePasswordDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public Guid? CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid? ModifyUserId { get; set; }

        public string ModifyUserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthUserRole> AuthUserRole { get; set; }
    }
}
