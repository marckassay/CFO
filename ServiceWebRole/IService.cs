﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WebJob;

namespace ServiceWebRole
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        IEnumerable<WOD> GetWOD(string DateEx);
    }
}
