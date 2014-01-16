﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QStrategyGUILib.MockupService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StrategyEngineUpdate", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(QStrategyGUILib.MockupService.StrategyResultsUpdate))]
    public partial class StrategyEngineUpdate : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private QStrategyGUILib.MockupService.APIState apiStateFieldField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public QStrategyGUILib.MockupService.APIState apiStateField {
            get {
                return this.apiStateFieldField;
            }
            set {
                if ((this.apiStateFieldField.Equals(value) != true)) {
                    this.apiStateFieldField = value;
                    this.RaisePropertyChanged("apiStateField");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StrategyResultsUpdate", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    [System.SerializableAttribute()]
    public partial class StrategyResultsUpdate : QStrategyGUILib.MockupService.StrategyEngineUpdate {
        
        private QStrategyGUILib.MockupService.StrategyOrder[] strategyOrderInfoListField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public QStrategyGUILib.MockupService.StrategyOrder[] strategyOrderInfoList {
            get {
                return this.strategyOrderInfoListField;
            }
            set {
                if ((object.ReferenceEquals(this.strategyOrderInfoListField, value) != true)) {
                    this.strategyOrderInfoListField = value;
                    this.RaisePropertyChanged("strategyOrderInfoList");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="APIState", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    public enum APIState : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Initial = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PendingLogon = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LoggedOn = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ready = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LogonFailure = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Disconnected = 5,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StrategyOrder", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    [System.SerializableAttribute()]
    public partial class StrategyOrder : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private double PnLField;
        
        private double max_LossField;
        
        private int number_Of_Open_OrdersField;
        
        private double pnL_Per_ShareField;
        
        private int position_SharesField;
        
        private double postion_AmountField;
        
        private double rebate_RevenueField;
        
        private int seedRemainingField;
        
        private QStrategyGUILib.MockupService.StrategyStatus statusField;
        
        private QStrategyGUILib.MockupService.StrategyInfo strategyField;
        
        private string symbolField;
        
        private double trading_RevenueField;
        
        private string[] user_MessageField;
        
        private int volumeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public double PnL {
            get {
                return this.PnLField;
            }
            set {
                if ((this.PnLField.Equals(value) != true)) {
                    this.PnLField = value;
                    this.RaisePropertyChanged("PnL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public double max_Loss {
            get {
                return this.max_LossField;
            }
            set {
                if ((this.max_LossField.Equals(value) != true)) {
                    this.max_LossField = value;
                    this.RaisePropertyChanged("max_Loss");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int number_Of_Open_Orders {
            get {
                return this.number_Of_Open_OrdersField;
            }
            set {
                if ((this.number_Of_Open_OrdersField.Equals(value) != true)) {
                    this.number_Of_Open_OrdersField = value;
                    this.RaisePropertyChanged("number_Of_Open_Orders");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public double pnL_Per_Share {
            get {
                return this.pnL_Per_ShareField;
            }
            set {
                if ((this.pnL_Per_ShareField.Equals(value) != true)) {
                    this.pnL_Per_ShareField = value;
                    this.RaisePropertyChanged("pnL_Per_Share");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int position_Shares {
            get {
                return this.position_SharesField;
            }
            set {
                if ((this.position_SharesField.Equals(value) != true)) {
                    this.position_SharesField = value;
                    this.RaisePropertyChanged("position_Shares");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public double postion_Amount {
            get {
                return this.postion_AmountField;
            }
            set {
                if ((this.postion_AmountField.Equals(value) != true)) {
                    this.postion_AmountField = value;
                    this.RaisePropertyChanged("postion_Amount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public double rebate_Revenue {
            get {
                return this.rebate_RevenueField;
            }
            set {
                if ((this.rebate_RevenueField.Equals(value) != true)) {
                    this.rebate_RevenueField = value;
                    this.RaisePropertyChanged("rebate_Revenue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int seedRemaining {
            get {
                return this.seedRemainingField;
            }
            set {
                if ((this.seedRemainingField.Equals(value) != true)) {
                    this.seedRemainingField = value;
                    this.RaisePropertyChanged("seedRemaining");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public QStrategyGUILib.MockupService.StrategyStatus status {
            get {
                return this.statusField;
            }
            set {
                if ((this.statusField.Equals(value) != true)) {
                    this.statusField = value;
                    this.RaisePropertyChanged("status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public QStrategyGUILib.MockupService.StrategyInfo strategy {
            get {
                return this.strategyField;
            }
            set {
                if ((object.ReferenceEquals(this.strategyField, value) != true)) {
                    this.strategyField = value;
                    this.RaisePropertyChanged("strategy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string symbol {
            get {
                return this.symbolField;
            }
            set {
                if ((object.ReferenceEquals(this.symbolField, value) != true)) {
                    this.symbolField = value;
                    this.RaisePropertyChanged("symbol");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public double trading_Revenue {
            get {
                return this.trading_RevenueField;
            }
            set {
                if ((this.trading_RevenueField.Equals(value) != true)) {
                    this.trading_RevenueField = value;
                    this.RaisePropertyChanged("trading_Revenue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string[] user_Message {
            get {
                return this.user_MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.user_MessageField, value) != true)) {
                    this.user_MessageField = value;
                    this.RaisePropertyChanged("user_Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int volume {
            get {
                return this.volumeField;
            }
            set {
                if ((this.volumeField.Equals(value) != true)) {
                    this.volumeField = value;
                    this.RaisePropertyChanged("volume");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StrategyInfo", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    [System.SerializableAttribute()]
    public partial class StrategyInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string strategyIdField;
        
        private string strategyNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string strategyId {
            get {
                return this.strategyIdField;
            }
            set {
                if ((object.ReferenceEquals(this.strategyIdField, value) != true)) {
                    this.strategyIdField = value;
                    this.RaisePropertyChanged("strategyId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string strategyName {
            get {
                return this.strategyNameField;
            }
            set {
                if ((object.ReferenceEquals(this.strategyNameField, value) != true)) {
                    this.strategyNameField = value;
                    this.RaisePropertyChanged("strategyName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StrategyStatus", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    public enum StrategyStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Trading = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Stopped = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Pause = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Max_Loss = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Hung = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProcessType", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    public enum ProcessType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        START = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        STOP = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CANCELALL = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UNWIND = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CancelOrderType", Namespace="http://schemas.datacontract.org/2004/07/MockupServer.Model")]
    public enum CancelOrderType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NA = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AllOpenOrders = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AllOrders = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MockupService.IMockupMainService")]
    public interface IMockupMainService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMockupMainService/GetStrategyUpdate", ReplyAction="http://tempuri.org/IMockupMainService/GetStrategyUpdateResponse")]
        QStrategyGUILib.MockupService.StrategyResultsUpdate GetStrategyUpdate();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMockupMainService/ProcessSymbol", ReplyAction="http://tempuri.org/IMockupMainService/ProcessSymbolResponse")]
        QStrategyGUILib.MockupService.StrategyResultsUpdate ProcessSymbol(string StrategyId, string[] symbol, QStrategyGUILib.MockupService.ProcessType processType, QStrategyGUILib.MockupService.CancelOrderType _cancelOrderType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMockupMainService/ProcessAllSymbol", ReplyAction="http://tempuri.org/IMockupMainService/ProcessAllSymbolResponse")]
        QStrategyGUILib.MockupService.StrategyResultsUpdate ProcessAllSymbol(string StrategyId, QStrategyGUILib.MockupService.ProcessType processType, QStrategyGUILib.MockupService.CancelOrderType _cancelOrderType);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMockupMainServiceChannel : QStrategyGUILib.MockupService.IMockupMainService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MockupMainServiceClient : System.ServiceModel.ClientBase<QStrategyGUILib.MockupService.IMockupMainService>, QStrategyGUILib.MockupService.IMockupMainService {
        
        public MockupMainServiceClient() {
        }
        
        public MockupMainServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MockupMainServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MockupMainServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MockupMainServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public QStrategyGUILib.MockupService.StrategyResultsUpdate GetStrategyUpdate() {
            return base.Channel.GetStrategyUpdate();
        }
        
        public QStrategyGUILib.MockupService.StrategyResultsUpdate ProcessSymbol(string StrategyId, string[] symbol, QStrategyGUILib.MockupService.ProcessType processType, QStrategyGUILib.MockupService.CancelOrderType _cancelOrderType) {
            return base.Channel.ProcessSymbol(StrategyId, symbol, processType, _cancelOrderType);
        }
        
        public QStrategyGUILib.MockupService.StrategyResultsUpdate ProcessAllSymbol(string StrategyId, QStrategyGUILib.MockupService.ProcessType processType, QStrategyGUILib.MockupService.CancelOrderType _cancelOrderType) {
            return base.Channel.ProcessAllSymbol(StrategyId, processType, _cancelOrderType);
        }
    }
}
