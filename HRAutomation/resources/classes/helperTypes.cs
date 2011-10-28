using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _HRAutomation.resources.classes
{
    /// <summary>
    /// These are basically the hard coded types
    /// that would represent real SFTP targets. If
    /// this application was used to submit SFTP encyrpted
    /// files to a healcare insurer the types might be like 
    /// "BlueCross", or "Kaiser". I have put fake samples here.
    /// </summary>
    public enum VendorType 
    {
        FakeProvider,
        SampleProvider,
        StillFakeProvider
    }

   
  
}
