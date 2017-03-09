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
        /// 拼音
        /// </summary>
        public string Spell { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool? Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 健康证过期
        /// </summary>
        public DateTime? HealthCertificateExpired { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// 学校
        /// </summary>
        public string School { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public bool? Enable { get; set; }
        /// <summary>
        /// 生日提醒
        /// </summary>
        public bool? RemindBirthday { get; set; }
        /// <summary>
        /// 健康证过期提醒
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
