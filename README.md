PowerBI.Api.Client
=======

C# Client library to deal with PowerBI Rest Api : [MSDN DOCUMENTATION](https://msdn.microsoft.com/en-us/library/dn877544)


## Installation

```
PM> Install-Package PowerBI.Api.Client
```

## Features

  * Automatic OAuth2 authentication
  * Datasets listing
  * Datasets & tables creation
  * Insert data into tables
  * Clean data from tables



## Getting started

To configure the PowerBI Client Api you must use the configuration section. Add it to the .config :

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<configSections>
		<section name="PowerBIConfiguration" type="PowerBI.Api.Client.Configuration.PowerBIConfiguration, PowerBI.Api.Client, Version=1.0.0.0"/>
	</configSections>
	<PowerBIConfiguration>
	    <OAuth
	      Authority="https://login.windows.net/common/oauth2/authorize" 
	      Resource="https://analysis.windows.net/powerbi/api"
	      Client="MyClientId" 
      	  User="MyUserName" 
      	  Password="MyPassword"/>
	    <Api 
	      Url="https://api.powerbi.com/beta/myorg/datasets" />
	  </PowerBIConfiguration>
</configuration>
```

Client is now ready. It's simple to use, call the **Do** method of **PowerBIClient** class to define an action which uses an authenticated instance.

```csharp
PowerBIClient.Do(api => {

});
```



## Api methods

**Get all Datasets**
```csharp
PowerBIClient.Do(api => {
	var datasets = api.GetDatasets();
});
```

**Get Dataset identifier by name**
```csharp
PowerBIClient.Do(api => {
	var datasetId = api.GetDatasetId("myDatasetName");
});
```

**Check if a name matches with a registered Dataset**
```csharp
PowerBIClient.Do(api => {
	var isDatasetExist = api.IsDatasetExist("myDatasetName");
});
```

**Check if an identifier matches with a registered Dataset**
```csharp
PowerBIClient.Do(api => {
	var isDatasetIdExist = api.IsDatasetIdExist("XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX");
});
```

**Create a Dataset and its related tables**
```csharp
PowerBIClient.Do(api => {
	var isCreated = api.CreateDataset("myDatasetName", typeof(MyObject1), typeof(MyObject2), ...);
});
```

**Insert a data into a table**
```csharp
PowerBIClient.Do(api => {
	var isObjectInsert = api.Insert("XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX", new MyObject1
	{
		DateTimeProp = DateTime.Now,
		IntProp = 1,
		BooleanProp = true,
		StringProp = "a string !",
		DoubleProp = 1.1
	});
});
```

**Insert a list of data into a table**
```csharp
PowerBIClient.Do(api => {
	var isListInsert = api.InsertAll("XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX", new List<object>
	{
		new MyObject1
		{
			DateTimeProp = DateTime.Now,
			IntProp = 1,
			BooleanProp = true,
			StringProp = "a string !",
			DoubleProp = 1.1
		},
		new MyObject1
		{
			DateTimeProp = DateTime.Now,
			IntProp = 2,
			BooleanProp = false,
			StringProp = "a string !",
			DoubleProp = 2.1
		}
	});
});
```

**Clean a table**
```csharp
PowerBIClient.Do(api => {
	var isDelete = api.Delete<Product>("XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX");
});
```



## More to come !
PowerBI & PowerBI Api are preview products. They are actively developed by Microsoft.
I hope they will add new features to the Rest api soon :)



## Licence

The MIT License (MIT)

Copyright (c) 2015 Sylvain PONTOREAU (pontoreau.sylvain@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



