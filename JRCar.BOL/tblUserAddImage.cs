//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JRCar.BOL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblUserAddImage
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public int tblUserAddID { get; set; }
    
        public virtual tblUserAdd tblUserAdd { get; set; }
    }
}
