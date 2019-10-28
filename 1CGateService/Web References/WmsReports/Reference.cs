﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Этот исходный текст был создан автоматически: Microsoft.VSDesigner, версия: 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace _1CGateService.WmsReports {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="aReportsSoapBinding", Namespace="http://www.amilen.ru/knit/reports")]
    public partial class aReports : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetLoadedArrivalPlanOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetStockBalanceOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLoadedShipmentPlanOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetInventoryActOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public aReports() {
            this.Url = global::_1CGateService.Properties.Settings.Default._1CGateService_WmsReports_aReports;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetLoadedArrivalPlanCompletedEventHandler GetLoadedArrivalPlanCompleted;
        
        /// <remarks/>
        public event GetStockBalanceCompletedEventHandler GetStockBalanceCompleted;
        
        /// <remarks/>
        public event GetLoadedShipmentPlanCompletedEventHandler GetLoadedShipmentPlanCompleted;
        
        /// <remarks/>
        public event GetInventoryActCompletedEventHandler GetInventoryActCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.amilen.ru/knit/reports#aReports:GetLoadedArrivalPlan", RequestNamespace="http://www.amilen.ru/knit/reports", ResponseNamespace="http://www.amilen.ru/knit/reports", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public LoadedArrivalPlanList GetLoadedArrivalPlan(System.DateTime BeginPeriod, System.DateTime EndPeriod) {
            object[] results = this.Invoke("GetLoadedArrivalPlan", new object[] {
                        BeginPeriod,
                        EndPeriod});
            return ((LoadedArrivalPlanList)(results[0]));
        }
        
        /// <remarks/>
        public void GetLoadedArrivalPlanAsync(System.DateTime BeginPeriod, System.DateTime EndPeriod) {
            this.GetLoadedArrivalPlanAsync(BeginPeriod, EndPeriod, null);
        }
        
        /// <remarks/>
        public void GetLoadedArrivalPlanAsync(System.DateTime BeginPeriod, System.DateTime EndPeriod, object userState) {
            if ((this.GetLoadedArrivalPlanOperationCompleted == null)) {
                this.GetLoadedArrivalPlanOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLoadedArrivalPlanOperationCompleted);
            }
            this.InvokeAsync("GetLoadedArrivalPlan", new object[] {
                        BeginPeriod,
                        EndPeriod}, this.GetLoadedArrivalPlanOperationCompleted, userState);
        }
        
        private void OnGetLoadedArrivalPlanOperationCompleted(object arg) {
            if ((this.GetLoadedArrivalPlanCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLoadedArrivalPlanCompleted(this, new GetLoadedArrivalPlanCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.amilen.ru/knit/reports#aReports:GetStockBalance", RequestNamespace="http://www.amilen.ru/knit/reports", ResponseNamespace="http://www.amilen.ru/knit/reports", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("return")]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("Lines", IsNullable=false)]
        public StockBalanceLine[] GetStockBalance(System.DateTime Date, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string OrganizationID, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string NomenclatureID) {
            object[] results = this.Invoke("GetStockBalance", new object[] {
                        Date,
                        OrganizationID,
                        NomenclatureID});
            return ((StockBalanceLine[])(results[0]));
        }
        
        /// <remarks/>
        public void GetStockBalanceAsync(System.DateTime Date, string OrganizationID, string NomenclatureID) {
            this.GetStockBalanceAsync(Date, OrganizationID, NomenclatureID, null);
        }
        
        /// <remarks/>
        public void GetStockBalanceAsync(System.DateTime Date, string OrganizationID, string NomenclatureID, object userState) {
            if ((this.GetStockBalanceOperationCompleted == null)) {
                this.GetStockBalanceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetStockBalanceOperationCompleted);
            }
            this.InvokeAsync("GetStockBalance", new object[] {
                        Date,
                        OrganizationID,
                        NomenclatureID}, this.GetStockBalanceOperationCompleted, userState);
        }
        
        private void OnGetStockBalanceOperationCompleted(object arg) {
            if ((this.GetStockBalanceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetStockBalanceCompleted(this, new GetStockBalanceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.amilen.ru/knit/reports#aReports:GetLoadedShipmentPlan", RequestNamespace="http://www.amilen.ru/knit/reports", ResponseNamespace="http://www.amilen.ru/knit/reports", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public LoadedShipmentPlanList GetLoadedShipmentPlan(System.DateTime BeginPeriod, System.DateTime EndPeriod) {
            object[] results = this.Invoke("GetLoadedShipmentPlan", new object[] {
                        BeginPeriod,
                        EndPeriod});
            return ((LoadedShipmentPlanList)(results[0]));
        }
        
        /// <remarks/>
        public void GetLoadedShipmentPlanAsync(System.DateTime BeginPeriod, System.DateTime EndPeriod) {
            this.GetLoadedShipmentPlanAsync(BeginPeriod, EndPeriod, null);
        }
        
        /// <remarks/>
        public void GetLoadedShipmentPlanAsync(System.DateTime BeginPeriod, System.DateTime EndPeriod, object userState) {
            if ((this.GetLoadedShipmentPlanOperationCompleted == null)) {
                this.GetLoadedShipmentPlanOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLoadedShipmentPlanOperationCompleted);
            }
            this.InvokeAsync("GetLoadedShipmentPlan", new object[] {
                        BeginPeriod,
                        EndPeriod}, this.GetLoadedShipmentPlanOperationCompleted, userState);
        }
        
        private void OnGetLoadedShipmentPlanOperationCompleted(object arg) {
            if ((this.GetLoadedShipmentPlanCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLoadedShipmentPlanCompleted(this, new GetLoadedShipmentPlanCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.amilen.ru/knit/reports#aReports:GetInventoryAct", RequestNamespace="http://www.amilen.ru/knit/reports", ResponseNamespace="http://www.amilen.ru/knit/reports", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public aInventoryActList GetInventoryAct(System.DateTime BeginPeriod, System.DateTime EndPeriod) {
            object[] results = this.Invoke("GetInventoryAct", new object[] {
                        BeginPeriod,
                        EndPeriod});
            return ((aInventoryActList)(results[0]));
        }
        
        /// <remarks/>
        public void GetInventoryActAsync(System.DateTime BeginPeriod, System.DateTime EndPeriod) {
            this.GetInventoryActAsync(BeginPeriod, EndPeriod, null);
        }
        
        /// <remarks/>
        public void GetInventoryActAsync(System.DateTime BeginPeriod, System.DateTime EndPeriod, object userState) {
            if ((this.GetInventoryActOperationCompleted == null)) {
                this.GetInventoryActOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInventoryActOperationCompleted);
            }
            this.InvokeAsync("GetInventoryAct", new object[] {
                        BeginPeriod,
                        EndPeriod}, this.GetInventoryActOperationCompleted, userState);
        }
        
        private void OnGetInventoryActOperationCompleted(object arg) {
            if ((this.GetInventoryActCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInventoryActCompleted(this, new GetInventoryActCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class LoadedArrivalPlanList {
        
        private System.DateTime beginPeriodField;
        
        private System.DateTime endPeriodField;
        
        private LoadedArrivalPlan[] documsField;
        
        /// <remarks/>
        public System.DateTime BeginPeriod {
            get {
                return this.beginPeriodField;
            }
            set {
                this.beginPeriodField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime EndPeriod {
            get {
                return this.endPeriodField;
            }
            set {
                this.endPeriodField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Docums")]
        public LoadedArrivalPlan[] Docums {
            get {
                return this.documsField;
            }
            set {
                this.documsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class LoadedArrivalPlan {
        
        private string documentIDField;
        
        private LoadedArrivalPlanLine[] linesField;
        
        /// <remarks/>
        public string DocumentID {
            get {
                return this.documentIDField;
            }
            set {
                this.documentIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Lines")]
        public LoadedArrivalPlanLine[] Lines {
            get {
                return this.linesField;
            }
            set {
                this.linesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class LoadedArrivalPlanLine {
        
        private string nomenclatureIDField;
        
        private string storangeUnitIDField;
        
        private string statusItemIDField;
        
        private bool defectField;
        
        private decimal quantityField;
        
        private decimal quantityUnitField;
        
        /// <remarks/>
        public string NomenclatureID {
            get {
                return this.nomenclatureIDField;
            }
            set {
                this.nomenclatureIDField = value;
            }
        }
        
        /// <remarks/>
        public string StorangeUnitID {
            get {
                return this.storangeUnitIDField;
            }
            set {
                this.storangeUnitIDField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemID {
            get {
                return this.statusItemIDField;
            }
            set {
                this.statusItemIDField = value;
            }
        }
        
        /// <remarks/>
        public bool Defect {
            get {
                return this.defectField;
            }
            set {
                this.defectField = value;
            }
        }
        
        /// <remarks/>
        public decimal Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        public decimal QuantityUnit {
            get {
                return this.quantityUnitField;
            }
            set {
                this.quantityUnitField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class aInventoryActLine {
        
        private string nomenclatureIDField;
        
        private string nomenclatureNameField;
        
        private string statusItemIDField;
        
        private string statusItemLackOfIDField;
        
        private string statusItemLackOfNameField;
        
        private string statusItemSurplusIDField;
        
        private string statusItemSurplusNameField;
        
        private bool defectField;
        
        private decimal quantityField;
        
        /// <remarks/>
        public string NomenclatureID {
            get {
                return this.nomenclatureIDField;
            }
            set {
                this.nomenclatureIDField = value;
            }
        }
        
        /// <remarks/>
        public string NomenclatureName {
            get {
                return this.nomenclatureNameField;
            }
            set {
                this.nomenclatureNameField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemID {
            get {
                return this.statusItemIDField;
            }
            set {
                this.statusItemIDField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemLackOfID {
            get {
                return this.statusItemLackOfIDField;
            }
            set {
                this.statusItemLackOfIDField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemLackOfName {
            get {
                return this.statusItemLackOfNameField;
            }
            set {
                this.statusItemLackOfNameField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemSurplusID {
            get {
                return this.statusItemSurplusIDField;
            }
            set {
                this.statusItemSurplusIDField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemSurplusName {
            get {
                return this.statusItemSurplusNameField;
            }
            set {
                this.statusItemSurplusNameField = value;
            }
        }
        
        /// <remarks/>
        public bool Defect {
            get {
                return this.defectField;
            }
            set {
                this.defectField = value;
            }
        }
        
        /// <remarks/>
        public decimal Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class aInventoryAct {
        
        private string documentIDField;
        
        private string typeDocumentIDField;
        
        private string typeDocumentNameField;
        
        private aInventoryActLine[] linesField;
        
        /// <remarks/>
        public string DocumentID {
            get {
                return this.documentIDField;
            }
            set {
                this.documentIDField = value;
            }
        }
        
        /// <remarks/>
        public string TypeDocumentID {
            get {
                return this.typeDocumentIDField;
            }
            set {
                this.typeDocumentIDField = value;
            }
        }
        
        /// <remarks/>
        public string TypeDocumentName {
            get {
                return this.typeDocumentNameField;
            }
            set {
                this.typeDocumentNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Lines")]
        public aInventoryActLine[] Lines {
            get {
                return this.linesField;
            }
            set {
                this.linesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class aInventoryActList {
        
        private System.DateTime beginPeriodField;
        
        private System.DateTime endPeriodField;
        
        private aInventoryAct[] documsField;
        
        /// <remarks/>
        public System.DateTime BeginPeriod {
            get {
                return this.beginPeriodField;
            }
            set {
                this.beginPeriodField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime EndPeriod {
            get {
                return this.endPeriodField;
            }
            set {
                this.endPeriodField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Docums")]
        public aInventoryAct[] Docums {
            get {
                return this.documsField;
            }
            set {
                this.documsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class аSerialNumber {
        
        private string serialNumberTextField;
        
        /// <remarks/>
        public string SerialNumberText {
            get {
                return this.serialNumberTextField;
            }
            set {
                this.serialNumberTextField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class LoadedShipmentPlanLine {
        
        private string nomenclatureIDField;
        
        private string storangeUnitIDField;
        
        private string statusItemIDField;
        
        private bool defectField;
        
        private decimal quantityField;
        
        private decimal quantityUnitField;
        
        private аSerialNumber[] serialNumbersField;
        
        /// <remarks/>
        public string NomenclatureID {
            get {
                return this.nomenclatureIDField;
            }
            set {
                this.nomenclatureIDField = value;
            }
        }
        
        /// <remarks/>
        public string StorangeUnitID {
            get {
                return this.storangeUnitIDField;
            }
            set {
                this.storangeUnitIDField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemID {
            get {
                return this.statusItemIDField;
            }
            set {
                this.statusItemIDField = value;
            }
        }
        
        /// <remarks/>
        public bool Defect {
            get {
                return this.defectField;
            }
            set {
                this.defectField = value;
            }
        }
        
        /// <remarks/>
        public decimal Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        public decimal QuantityUnit {
            get {
                return this.quantityUnitField;
            }
            set {
                this.quantityUnitField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SerialNumbers", IsNullable=true)]
        public аSerialNumber[] SerialNumbers {
            get {
                return this.serialNumbersField;
            }
            set {
                this.serialNumbersField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class LoadedShipmentPlan {
        
        private string documentIDField;
        
        private LoadedShipmentPlanLine[] linesField;
        
        /// <remarks/>
        public string DocumentID {
            get {
                return this.documentIDField;
            }
            set {
                this.documentIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Lines")]
        public LoadedShipmentPlanLine[] Lines {
            get {
                return this.linesField;
            }
            set {
                this.linesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class LoadedShipmentPlanList {
        
        private System.DateTime beginPeriodField;
        
        private System.DateTime endPeriodField;
        
        private LoadedShipmentPlan[] documsField;
        
        /// <remarks/>
        public System.DateTime BeginPeriod {
            get {
                return this.beginPeriodField;
            }
            set {
                this.beginPeriodField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime EndPeriod {
            get {
                return this.endPeriodField;
            }
            set {
                this.endPeriodField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Docums")]
        public LoadedShipmentPlan[] Docums {
            get {
                return this.documsField;
            }
            set {
                this.documsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.amilen.ru/knit/reports")]
    public partial class StockBalanceLine {
        
        private string nomenclatureIDField;
        
        private string statusItemIDField;
        
        private bool defectField;
        
        private System.DateTime expirationDateField;
        
        private decimal quantityField;
        
        private bool setField;
        
        /// <remarks/>
        public string NomenclatureID {
            get {
                return this.nomenclatureIDField;
            }
            set {
                this.nomenclatureIDField = value;
            }
        }
        
        /// <remarks/>
        public string StatusItemID {
            get {
                return this.statusItemIDField;
            }
            set {
                this.statusItemIDField = value;
            }
        }
        
        /// <remarks/>
        public bool Defect {
            get {
                return this.defectField;
            }
            set {
                this.defectField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime ExpirationDate {
            get {
                return this.expirationDateField;
            }
            set {
                this.expirationDateField = value;
            }
        }
        
        /// <remarks/>
        public decimal Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Set {
            get {
                return this.setField;
            }
            set {
                this.setField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetLoadedArrivalPlanCompletedEventHandler(object sender, GetLoadedArrivalPlanCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLoadedArrivalPlanCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLoadedArrivalPlanCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public LoadedArrivalPlanList Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((LoadedArrivalPlanList)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetStockBalanceCompletedEventHandler(object sender, GetStockBalanceCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetStockBalanceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetStockBalanceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public StockBalanceLine[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((StockBalanceLine[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetLoadedShipmentPlanCompletedEventHandler(object sender, GetLoadedShipmentPlanCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLoadedShipmentPlanCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLoadedShipmentPlanCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public LoadedShipmentPlanList Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((LoadedShipmentPlanList)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetInventoryActCompletedEventHandler(object sender, GetInventoryActCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInventoryActCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetInventoryActCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public aInventoryActList Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((aInventoryActList)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591