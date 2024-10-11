# required endpoints

export const deleteBlogPost = async (id: number) => {
  try {
    const res = await fetch(
      `http://localhost:${localhostPort}/api/BlogPost/${id}`,
      {
        method: "DELETE",
      },
    );
    return await res.json();
  } catch (e) {
    console.log("error", e);
    return [];
  }
};

export const patchBlogPost = async (req: EditBlogPostReqBody) => {
  try {
    const res = await fetch(
      `http://localhost:${localhostPort}/api/BlogPost/${req.Id}`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(req),
      },
    );

    return { status: res.status, content: req };
  } catch (e) {
    console.log("error", e);
    return { status: 500, error: e };
  }
};
