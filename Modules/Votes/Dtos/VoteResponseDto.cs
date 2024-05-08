namespace CourseWork.Modules.Votes.Dtos
{
    public class VoteResponseDto
    {
        public required int Id { get; set; }


    }

    public class GetVoteResponseDto
    {
        public int? Id { get; set; }

        public bool? IsUpVote { get; set; }
    }
}
