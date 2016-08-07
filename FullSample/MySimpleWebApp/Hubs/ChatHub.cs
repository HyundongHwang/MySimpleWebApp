using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySimpleWebApp.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public string concatStr(string str1, string str2)
        {
            return str1 + str2;
        }

        public class MyOrder
        {
            public string Menu { get; set; }
            public int Price { get; set; }
            public int Count { get; set; }
        }

        public List<MyOrder> getMyOrders()
        {
            var result = new List<MyOrder>();
            result.Add(new MyOrder { Menu = "짜장면", Price = 5000, Count = 10 });
            result.Add(new MyOrder { Menu = "짬뽕", Price = 1000, Count = 20 });
            result.Add(new MyOrder { Menu = "탕수육", Price = 1500, Count = 30 });
            return result;
        }
    }
}