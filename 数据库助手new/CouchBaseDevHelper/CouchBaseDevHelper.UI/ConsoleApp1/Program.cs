using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new FrameWorkMemcachedClientLib.MemClient("10.252.254.105", 11001, "");
            client = new FrameWorkMemcachedClientLib.MemClient("10.252.254.105", 11001, "");
        }
    }
}
