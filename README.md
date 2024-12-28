# cSharp-webserver-personal-blog
A web server running C# / dotNET to handle API requests for a personal blogging website.

## Run locally and get Documentation

1. Within ./v2 directory create a env file using 'touch .env' and add the following values for a psql database.
- HOST=
- DATABASE=
- USERNAME=
- PASSWORD=

2. When running in dotNET press `CTRL + F5` and navigate to `https://localhost:5000/swagger/index.html`.

Always use V2 code as V1 is deprecated.

## Table of Contents
1. [Introduction](#introduction)
2. [General Information](#general-information)
3. [Blog Posts](#blog-posts)
4. [Images](#images)
5. [Getting Started](#getting-started)

## <a name="introduction"></a> Introduction
This API provides endpoints for managing blog posts and images in a minimal ASP.NET application.

## <a name="general-information"></a> General Information
The following table summarizes the key information about this API:

| Key | Value |
|---|---|
| **API Base URL** | `/api` |
| **Supported Methods** | GET, POST, PATCH, DELETE |
| **OpenAPI Documentation** | Available at `/swagger` in a web browser |

## <a name="blog-posts"></a> Blog Posts
The following table describes the endpoints for managing blog posts.

| Endpoint         | Method | Description                          |
| ---------------- | ------- | ------------------------------------ |
| `/api/ping`       | GET     | Returns "pong" response              |
| `/api/author`      | GET     | Retrieves an author item             |
| `/api/posts`      | GET     | Retrieves all blog posts             |
| `/api/post/{id}`  | POST    | Creates a new blog post              |
| `/api/post/{id}`  | DELETE   | Deletes an existing blog post        |
| `/api/post/{id}`  | PATCH    | Updates an existing blog post        |

### Author Item
The following properties are supported for the author item.

| Property     | Type      | Description                       |
| ------------- | --------- | --------------------------------- |
| `Name`         | string   | The name of the author             |
| `Username`     | string   | The username of the author        |

### Blog Post
The following properties are supported for a blog post.

| Property       | Type     | Description                          |
| -------------- | -------- | ------------------------------------ |
| `Id`            | int      | The unique identifier of a blog post  |
| `Title`         | string   | The title of the blog post            |
| `Content`       | string   | The content of the blog post          |
| `Author`      | int      | The identifier of the author          |
| `Likes`       | int    | Number of likes|

## <a name="images"></a> Images
The following table describes the endpoints for managing images.

| Endpoint         | Method | Description                          |
| ---------------- | ------- | ------------------------------------ |
| `/api/images`     | GET     | Retrieves all images                |
| `/api/image/{id}` | GET     | Retrieves an image by its identifier |
| `/api/image/{id}` | POST    | Uploads a new image                  |
| `/api/image/{id}` | DELETE   | Deletes an existing image            |

### Image
The following properties are supported for an image.

| Property     | Type      | Description                       |
| ------------- | --------- | --------------------------------- |
| `Id`          | int      | The unique identifier of an image   |
| `blogpost_id` | int      | Foreign key                |
| `Name`         | string   | Name of image from file name              |
| `Img`         | bytes[]   | Image bytes              |

## <a name="getting-started"></a> Getting Started
To get started with this API, clone the repository and run the application using your preferred .NET CLI or IDE. The API will be available at `http://localhost:<port>/api`.

Make sure to install packages like Swagger and Swagger UI to test and explore the API using a web browser. For example, you can use Postman to make requests against the API endpoints.
