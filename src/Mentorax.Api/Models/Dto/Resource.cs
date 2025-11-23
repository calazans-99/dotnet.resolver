using System.Collections.Generic;

namespace Mentorax.Api.Models.Dto
{
    public class Resource<T>
    {
        public T Data { get; set; }
        public List<LinkDto> Links { get; set; } = new();

        public Resource(T data)
        {
            Data = data;
        }
    }
}
