using System;
using System.Collections.Generic;
using System.Data.Entity;
using MyCupOfCoffeeService.DataObjects;
using MyCupOfCoffeeService.Models;

namespace MyCupOfCoffeeService
{
    public class MyCupOfCoffeeInitializer : CreateDatabaseIfNotExists<MyCupOfCoffeeContext>
    {
        protected override void Seed(MyCupOfCoffeeContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}