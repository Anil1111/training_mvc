using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingApp.CRM
{
    public class CrmService
    {

        private IOrganizationService _orgService;
        public CrmServiceClient conn;
        public static IConfigurationRoot Configuration { get; set; }



        public CrmService()
        {
            // Connect to the CRM web service using a connection string.

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json");

            Configuration = builder.Build();

             var credentials = new System.Net.NetworkCredential("eperformanceinc\\student1", "Eperformance01", "");
            conn = new CrmServiceClient(credentials,
               "stu16-01.crm.eperformanceinc.com",
               "443",
               "stu16-01", useUniqueInstance: true, useSsl: true);



            //CrmServiceClient conn = new CrmServiceClient("Url = https://eperformanceinc1.crm.dynamics.com/; Username=fpearson@eperformanceinc.com; Password=Gaby614$; authtype=Office365");
           
            Console.WriteLine("Connection is ready????? " + conn.IsReady.ToString());
            _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;



            //Create any entity records this sample requires.
        }

        public IOrganizationService GetService()
        {
            IOrganizationService service = _orgService;
            return service;
        }

        public static IOrganizationService GetServiceProvider()
        {
            var crmService = new CrmService();
            IOrganizationService service = crmService._orgService;
            return service;
        }

        public static string GetTestUserInfo()
        {
            var crmService = new CrmService();
            IOrganizationService service = crmService._orgService;
            //// Obtain information about the logged on user from the web service.
            Guid userid = ((WhoAmIResponse)service.Execute(new WhoAmIRequest())).UserId;
            // Entity account = _orgService.Retrieve("Account", Guid.NewGuid(), new ColumnSet(new String[] { "accountid", "accountname" }));
            Entity systemUser = service.Retrieve("systemuser", userid,
                new ColumnSet(new string[] { "firstname", "lastname" }));
            var user = String.Format("Logged on user is {0} {1}.", systemUser.GetAttributeValue<string>("firstname"), systemUser.GetAttributeValue<string>("lastname"));
            //return conn.IsReady.ToString();
            return user;

        }

        public string GetTestUser()
        {
            //// Obtain information about the logged on user from the web service.
            Guid userid = ((WhoAmIResponse)_orgService.Execute(new WhoAmIRequest())).UserId;
            // Entity account = _orgService.Retrieve("Account", Guid.NewGuid(), new ColumnSet(new String[] { "accountid", "accountname" }));
            Entity systemUser = _orgService.Retrieve("systemuser", userid,
                new ColumnSet(new string[] { "firstname", "lastname" }));
            var user = String.Format("Logged on user is {0} {1}.", systemUser.GetAttributeValue<string>("firstname"), systemUser.GetAttributeValue<string>("lastname"));
            //return conn.IsReady.ToString();
            return user;
            // Cast the proxy client to the IOrganizationService interface.
        }

        internal object RetrieveMultiple(QueryExpression qe)
        {
            throw new NotImplementedException();
        }
    }
}
