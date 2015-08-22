# EqualityAssert
Helps testing your classes equality members.

## Whats tested
```C#
public bool Equals(object other);
public int GetHashCode();
public bool Equals<T>(T other);
public static bool operator== (YourClass first, YourClass second);
public static bool operator!= (YourClass first, YourClass second);
```
