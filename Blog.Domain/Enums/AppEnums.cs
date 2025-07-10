namespace Blog.Domain.Enums;

public enum Status
{
    Draft,
    Review,
    Published,
    Restricted,
    Deleted
}

public enum Categories
{
    None
}

public enum UserRole
{
    Viewer,
    PremiumUser,
    Author,
    Moderator,
    Admin
}