using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CFOService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        SimpleWODObject GetWOD();
    }
}
