
using JAngine;
using JAngine.ECS;

var world = new World();

for (int i = 0; i < 10_000; i++)
{
    world.CreateEntity(i);
    world.CreateEntity(i, MathF.Log2(i));
}

Random rand = new Random(0);
for (int i = 0; i < 10_000; i++)
{
    using (Log.Time("Find entity", false))
    {
        int first = world.GetComponents<int>().ToList().Find(i =>  i.Equals(rand.Next(0, 10_000)));
    }
}
Log.LogTimer("Find entity");
