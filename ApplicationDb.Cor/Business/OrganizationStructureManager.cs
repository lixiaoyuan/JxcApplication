using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class OrganizationStructureManager
    {
        private ApplicationDbContext applicationDbContext;
        public static OrganizationStructureManager Create()
        {
            return new OrganizationStructureManager();
        }

        private OrganizationStructureManager()
        {
            applicationDbContext = new ApplicationDbContext();
        }

        public ObservableCollection<Organization> QueOrganizations()
        {
            applicationDbContext.Organization.Load();
            return applicationDbContext.Organization.Local;
        }

        public bool Add(Organization data)
        {
            applicationDbContext.Organization.Add(data);
            return applicationDbContext.SaveChanges() > 0;
        }

        public ObservableCollection<SystemUser> GetOrganizationUserCheck(Guid organizationId)
        {
            return applicationDbContext.GetOrganizationUserCheck(organizationId).ToObservableCollection();
        }

        public string MaxCode()
        {
            return applicationDbContext.Organization.Max(a => a.Code);
        }

        public string UpdateOrganizationUser(Guid organId, string userIds)
        {
            var outMsg=new SqlParameter("Msg",SqlDbType.VarChar,200);
            outMsg.Direction=ParameterDirection.Output;
            applicationDbContext.UpdateOrganizationUser(organId, userIds, outMsg);
            return outMsg.Value.ToString();
        }
    }
}
