﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AsbaBank.Presentation.Shell.QueryServices {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="Asba.Queries", ConfigurationName="QueryServices.QueryService")]
    public interface QueryService {
        
        [System.ServiceModel.OperationContractAttribute(Action="Asba.Queries/QueryService/Handle", ReplyAction="Asba.Queries/QueryService/HandleResponse")]
        string Handle(string queryString);
        
        [System.ServiceModel.OperationContractAttribute(Action="Asba.Queries/QueryService/Handle", ReplyAction="Asba.Queries/QueryService/HandleResponse")]
        System.Threading.Tasks.Task<string> HandleAsync(string queryString);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface QueryServiceChannel : AsbaBank.Presentation.Shell.QueryServices.QueryService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class QueryServiceClient : System.ServiceModel.ClientBase<AsbaBank.Presentation.Shell.QueryServices.QueryService>, AsbaBank.Presentation.Shell.QueryServices.QueryService {
        
        public QueryServiceClient() {
        }
        
        public QueryServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public QueryServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QueryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QueryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Handle(string queryString) {
            return base.Channel.Handle(queryString);
        }
        
        public System.Threading.Tasks.Task<string> HandleAsync(string queryString) {
            return base.Channel.HandleAsync(queryString);
        }
    }
}
