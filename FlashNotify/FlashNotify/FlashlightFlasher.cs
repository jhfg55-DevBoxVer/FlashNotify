using System;
using System.Threading.Tasks;
using Windows.Devices.Lights;

namespace FlashNotify;

public static class FlashlightFlasher
{
    public static async void FlashFlashlight(Settings settings)
    {
        var lamp = await Lamp.GetDefaultAsync();
        if (lamp == null) return;

        for (int i = 0; i < settings.FlashlightFlashCount; i++)
        {
            lamp.IsEnabled = true;
            await Task.Delay(settings.FlashlightDuration);
            lamp.IsEnabled = false;
            if (i < settings.FlashlightFlashCount - 1)
            {
                await Task.Delay(settings.FlashlightDuration);
            }
        }
    }
}
