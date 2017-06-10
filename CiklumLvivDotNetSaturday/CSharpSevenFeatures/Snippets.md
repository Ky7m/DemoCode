```charp
var root = TreeNode<string>.BuildSampleTree();
```
```charp
Action<TreeNode<string>, Action<string>> traverse = null;
            traverse = (node, action) =>
            {
                action(node.Data);
                if (node.HasChild)
                {
                    foreach (var n in node.Children)
                    {
                        traverse(n, action);
                    }
                }
            };

            traverse(root, Console.WriteLine);
```
```charp
 void Traverse(TreeNode<string> node, Action<string> action)
            {
                action(node.Data);
                if (node.HasChild)
                {
                    foreach (var n in node.Children)
                    {
                        Traverse(n, action);
                    }
                }
            }

            Traverse(root, Console.WriteLine);
```
```charp
var value = 5;
            object someValue = value;

            if (someValue is int i)
            {
                Console.WriteLine($"it is int and value is {i}");
            }

            if (someValue is string s)
            {
                Console.WriteLine($"it is string and value is {s}");
            }
```
```charp
switch (someValue)
            {

                case 5:
                    Console.WriteLine("it is 5.");
                    break;
            }
```
```charp
default:
                    Console.WriteLine("default.");
                    break;
```
```charp
 case *:
                    Console.WriteLine("default.");
                    break;
```
```charp
            var root = TreeNode<string>.BuildSampleTree();
            switch (root)
            {

            }
```
```charp
 case TreeNode<string> node:
                    Console.WriteLine("some node");
                    break;
```
```charp
case null:
                    Console.WriteLine("null");
                    break;
```
```charp
case *:
                    break;
```
```charp
 case TreeNode<string> node when node.HasChild:
                    Console.WriteLine("some node with children");
                    break;
```
```charp
 var array = new[] { 1, 2, 3, 4, 2, 3, 5, 6, 7, 8 };

            var element = Find(array, x => x == 5);
            Console.WriteLine(element);
```
```charp
private static int Find(int[] array, Predicate<int> match)
        {
            int i = 0;
            for (i = 0; i < array.Length; i++)
            {
                if (match(array[i]))
                {
                    break;
                }
            }

            return i + 1;
        }
```
```charp
element = 6;
            Console.WriteLine($"{element} == {array[7]}");
```