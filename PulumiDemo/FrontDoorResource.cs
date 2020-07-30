using Pulumi;
using Pulumi.Azure.FrontDoor;
using Pulumi.Azure.FrontDoor.Inputs;

namespace PulumiDemo
{
    public class FrontDoorResource : ComponentResource
    {
        [Output] public Output<string> Cname { get; private set; }
        public FrontDoorResource(string name, Input<string> resourceGroupName, string defaultName, InputList<FrontdoorBackendPoolBackendArgs> backendPoolList, ComponentResourceOptions options = null) 
            : base("resource:azure:frontdoor", name, options)
        {
            var frontDoor = new Frontdoor(name, new FrontdoorArgs
            {
                Name = name,
                ResourceGroupName = resourceGroupName,
                FriendlyName = defaultName,
                EnforceBackendPoolsCertificateNameCheck = false,
                FrontendEndpoints = new FrontdoorFrontendEndpointArgs {Name = name, HostName = $"{name}.azurefd.net"},
                BackendPools = new FrontdoorBackendPoolArgs {Name = name, HealthProbeName = name, LoadBalancingName = name, Backends = backendPoolList},
                BackendPoolHealthProbes = new FrontdoorBackendPoolHealthProbeArgs {Name = name, Path = "/hc", Protocol = "Https", IntervalInSeconds = 30},
                BackendPoolLoadBalancings = new FrontdoorBackendPoolLoadBalancingArgs {Name = name},
                RoutingRules = new FrontdoorRoutingRuleArgs
                {
                    Name = name,
                    AcceptedProtocols = "Https",
                    PatternsToMatches = "/*",
                    FrontendEndpoints = name,
                    ForwardingConfiguration = new FrontdoorRoutingRuleForwardingConfigurationArgs {BackendPoolName = name, CacheUseDynamicCompression = false, ForwardingProtocol = "HttpsOnly"}
                }
            });

            Cname = frontDoor.Cname;
        }
    }
}