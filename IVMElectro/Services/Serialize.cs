using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IVMElectro.Models;
using IVMElectro.Services;

namespace IVMElectro.Services {
    sealed class Serialize {
        public static void SerializeRun(object target, bool fixName) {
            //if ( target is OutputData ) {
            //    List<string> aggregate = new List<string>(((OutputData)target).Get_dataInput);
            //    aggregate.AddRange(((OutputData)target).Get_dataMachine);
            //    aggregate.AddRange(((OutputData)target).Get_magneticCircuit);
            //    aggregate.AddRange(((OutputData)target).Get_idle);
            //    aggregate.AddRange(((OutputData)target).Get_nominalRating);
            //    aggregate.AddRange(((OutputData)target).Get_overloadCapacity);
            //    aggregate.AddRange(((OutputData)target).Get_startingConditions);
            //    if (fixName)
            //        ServiceIO.SaveDataToFile_fixName(aggregate);
            //    else
            //        ServiceIO.SaveDataToFile(aggregate);
            //}
            //if(target is InputData_Single) {
            //    if (fixName)
            //        ServiceIO.SaveObjectToXMLFile_fixName(target);
            //    else
            //        ServiceIO.SaveObjectToXMLFile(target);
            //}
        }
    }
}
