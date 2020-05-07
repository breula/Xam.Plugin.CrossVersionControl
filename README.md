# Xam.Plugin.CrossVersionControl
 Plugin do check last version on Apple and Android Stores for Xamarin application

# Use sample
Easily verify current app version published in the public stores as Play Store and Apple Store.

## Supported platforms and versions

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|7.0+|
|Xamarin.Android|Yes|10.0+|

## API Usage

### Check for latest version

Check the latest version available in the public store:

```csharp
var packageName = Xamarin.Essentials.AppInfo.PackageName;
var verifyVersion = Xam.Plugin.CrossVersionControl.CheckVersion.VerifyAndroid(packageName);
```
Also, you can directly write the package name in the "packageName" parameter.

### Full Example using Xamarin.Essentials

```csharp
var platform = Xamarin.Essentials.DeviceInfo.Platform;
var packageName = Xamarin.Essentials.AppInfo.PackageName;
var installedVersion = Xamarin.Essentials.AppInfo.Version.ToString();
var needsUpdate = false;
if (platform == DevicePlatform.Android)
{
    var verifyVersion = Xam.Plugin.CrossVersionControl.CheckVersion.VerifyAndroid(packageName);
    if (verifyVersion.VersioningType != Xam.Plugin.CrossVersionControl.VersioningType.NotIdentified)
    {

        if (verifyVersion.VersioningType == Xam.Plugin.CrossVersionControl.VersioningType.ByCodeName)
        {
            if (installedVersion != verifyVersion.StoreCurrentVersion)
            {
                needsUpdate = true;
            }
        }
        else
        {
            if (Convert.ToDouble(installedVersion) < Convert.ToDouble(verifyVersion.StoreCurrentVersion))
            {
                needsUpdate = true;
            }
        }
    }
}
if (needsUpdate)
{
    var update = await Application.Current.MainPage.DisplayAlert(
        "Nova Versão", 
        "Existe uma nova versão do seu App e é necessário atualizá-lo! Se escolher não o apicativo será encerrado.", 
        "Sim", 
        "Não");
    if (update)
    {
        await CrossLatestVersion.Current.OpenAppInStore();
    }
}
```

### Get installed version number

Get the version number of the current app's installed version:

```csharp
var installedVersion = Xamarin.Essentials.AppInfo.Version.ToString();
```

### Open app in public store

Open the current running app in the public store using CrossLatestVersion plugin:

```csharp
await CrossLatestVersion.Current.OpenAppInStore();
```

## License

Licensed under MIT. See [License file](https://github.com/edsnider/LatestVersionPlugin/blob/master/LICENSE)
