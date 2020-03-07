# JsonCryption.Newtonsoft
## Field Level Encryption (FLE) plugin for Newtonsoft.Json
JsonCryption.Newtonsoft offers Field Level Encryption (FLE) when serializing/deserializing between .NET objects and JSON.

### Installation
Package Manager:
```
Install-Package JsonCryption.Newtonsoft
```

.NET CLI:
```
dotnet add package JsonCryption.Newtonsoft
```

### Motivation
Field Level Encryption of C# objects during JSON serialization/deserialization should be:
- Relatively easy to use
- Powered by industry-standard cryptography best-practices

#### Relatively Easy to Use
With default configuration, encrypting a field/property just requires decorating it with `EncryptAttribute`, and serializing the object as usual:
```
// decorate properties to be encrypted
class Foo
{
    [Encrypt]
    public string MySecret { get; set; }
}

// serialize as normal
Foo foo = new Foo() { ... };

JsonSerializer serializer = ...
using TextWriter textWriter = ...
serializer.Serialize(textWriter, foo);
```

More details on usage scenarios can be found below.

#### Industry-standard Cryptography
Currently, JsonCryption.Newtonsoft is built on top of the `Microsoft.AspNetCore.DataProtection` library for handling encryption-related responsibilities:
- Encryption/decryption
- Key management
- Algorithm management
- etc.

Internally, we only depend on the two interfaces `IDataProtector` and `IDataProtectionProvider`. If you don't want to use Microsoft's implementations, you could just depend on `Microsoft.AspNetCore.DataProtection.Abstractions` and provide alternative implementations of `IDataProtector` and `IDataProtectionProvider`. One use case for this functionality might be creating a segregated `IDataProtector` per user, potentially making it easy to support GDPR's "right to forget" user data.

### Supported Types
JsonCryption.Newtonsoft should support any type serializable by Newtonsoft.Json. If you spot a missing type, please let me know (or better yet, create a PR!).

### Getting Started
#### Configuration
##### Step 1: Configure Microsoft.AspNetCore.DataProtection
JsonCryption.Newtonsoft depends on the `Microsoft.AspNetCore.DataProtection` library. Therefore, you should first ensure that your DataProtection layer is [configured properly](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/).

Next, configuration depends on the JSON serializer used...

##### Step 2: Configure Newtonsoft.Json
To configure JsonCryption.Newtonsoft with dependency injection, you'll need to register your default JsonSerializer with our `EncryptedContractResolver`:
```
// pseudo code
container.Register<JsonSerializer>(() => new JsonSerializer()
{
    ContractResolver = new EncryptedContractResolver(container.Resolve<IDataProtectionProvider>())
});
```

#### Usage
Once configured, using JsonCryption.Newtonsoft is just a matter of decorating the properties/fields you wish to encrypt and the `EncryptAttribute` and serializing your C# objects as you normally would:
```
class Foo
{
    [Encrypt]
    public string EncryptedString { get; }
  
    public string UnencryptedString { get; }
}

var myFoo = new Foo("some important value", "something very public");

// serializing
var serializer = ...
var builder = new StringBuilder();
using var textWriter = new StringWriter(builder);
serializer.Serialize(textWriter, myFoo);
var json = builder.ToString();

// deserializing
using var textReader = new StringReader(json);
using var reader = new JsonTextReader(textReader);
var deserialized = serializer.Deserialize<Foo>(json);
```

### Special Stuff
We're trying to maintain feature parity when it comes to annotations, but I'm likely missing finer details in places. Let me know if you spot something. When in doubt, verify what's possible via the tests

#### Non-public Properties and Fields
The easiest way to do this is to decorate the field/property with an additional `JsonPropertyAttribute`:
```
class NonPublicFoo
{
    [Encrypt]
    [JsonProperty]
    internal string InternalProperty { get; set; }
  
    [Encrypt]
    [JsonProperty]
    protected bool ProtectedField;
  
    [Encrypt]
    [JsonProperty]
    private Guid PrivateProperty { get; set; }
}
```

### Future Plans
JsonCryption.Newtonsoft is open to PRs and more regular contributors. Feel free to reach out if you're interested in helping.

Next, I'm hoping to do some benchmarking...

