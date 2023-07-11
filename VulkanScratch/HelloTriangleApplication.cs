using Glfw;
using Vulkan;
using VulkanScratch.Extensions;

public sealed class HelloTriangleApplication : IDisposable
{
    private IntPtr _window;
    private VkInstance _instance;
    
    public HelloTriangleApplication()
    {
        InitWindow();
        InitVulkan();
    }

    private void InitWindow()
    {
        if (!Glfw3.Init())
        {
            throw new Exception();
        }
        Glfw3.WindowHint(WindowAttribute.ClientApi, Constants.NoApi);
        Glfw3.WindowHint(WindowAttribute.Resizable, 0);
        _window = Glfw3.CreateWindow(Program.Width, Program.Height, "Vulkan window", MonitorHandle.Zero, IntPtr.Zero);
    }

    private void InitVulkan()
    {
        CreateInstance();
    }

    private unsafe void CreateInstance()
    {
        VkApplicationInfo appInfo = new VkApplicationInfo()
        {
            sType = VkStructureType.ApplicationInfo,
            pNext = IntPtr.Zero,
            pApplicationName = "Vulkan window",
            applicationVersion = MakeApiVersion(0, 1, 0, 0),
            pEngineName = "Vulkan engine",
            engineVersion = MakeApiVersion(0, 1, 0, 0),
            apiVersion = MakeApiVersion(0, 1, 0, 0),
        };

        IntPtr extensions = Glfw3.GetRequiredInstanceExtensions(out uint extensionCount);
        VkInstanceCreateInfo createInfo = new VkInstanceCreateInfo()
        {
            sType = VkStructureType.InstanceCreateInfo,
            pApplicationInfo = appInfo,
            enabledExtensionCount = extensionCount,
            ppEnabledExtensionNames =  extensions,
            enabledLayerCount = 0,
        };
        if (Vk.vkCreateInstance(ref createInfo, IntPtr.Zero, out VkInstance instance) != VkResult.Success)
        {
            throw new Exception("Failed to create instance.");
        }
    }

    private uint MakeApiVersion(uint variant, uint major, uint minor, uint patch)
    {
        return variant << 29 | major << 22 | minor << 12 | patch;
    }

    public void Run()
    {
        while (!Glfw3.WindowShouldClose(_window))
        {
            Glfw3.PollEvents();
        }
    }

    public void Dispose()
    {
        Glfw3.DestroyWindow(_window);
        Glfw3.Terminate();
        // TODO: Automatically destroy vulkan objects.
    }
}