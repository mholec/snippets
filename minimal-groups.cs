var apiGroup = app.MapGroup("api");
var coursesGroup = apiGroup.WithTags("Kurzy")

coursesGroup.MapGet("courses", // ...
coursesGroup.MapGet("courses/{id:int}", // ...