using Blog.Models;

namespace BlogPost.Data {

public class ExampleBlogPosts {

BlogPostItem[] ExampleBlogPostsArr = {new BlogPostItem(0, "Hiroji", "Why are Huskys so cute!", "Huskies are often considered adorable due to their distinct features and charming personalities. With their striking blue or multicolored eyes, thick coats, and expressive faces, huskies possess a natural allure that captures the hearts of many. Their fluffy appearance, combined with their playful and friendly nature, creates an instant appeal. Whether it's their mischievous antics, their infectious energy, or their affectionate demeanor, huskies possess a certain charm that brings joy and warmth to those who encounter them. Their cuteness goes beyond their physical attributes and resonates with their spirited and endearing personalities, making them irresistible to many animal lovers.", 0),

 new BlogPostItem(1, "Hiroji", "This api", "ASP.NET and C# are powerful technologies that work hand in hand to create robust and dynamic web applications. ASP.NET is a web development framework that allows developers to build feature-rich websites and web services. It provides a wide range of tools, libraries, and components that simplify the process of creating web applications. C#, on the other hand, is a versatile and modern programming language that is used extensively in the development of ASP.NET applications. With its object-oriented programming paradigm, C# provides developers with a clean and structured approach to building scalable and maintainable code. The combination of ASP.NET and C# enables developers to leverage a vast ecosystem of frameworks, libraries, and tools to create high-performance web applications that can handle complex functionality and provide an exceptional user experience. Together, they form a powerful duo that empowers developers to bring their web development ideas to life.", 0)
};

public BlogPostItem[] getData() {
    return ExampleBlogPostsArr;
}
}
}

