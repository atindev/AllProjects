using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;

namespace WAS.Application.Extensions
{
    public static class ProblemDetailsResponseExtensions
    {
        public static Exception Exception(this ProblemDetailsResponse problemDetailsResponse)
        {
            switch (problemDetailsResponse.Status)
            {
                case 400:
                    return new BadRequestException(problemDetailsResponse.Detail);
                case 404:
                    return new NotFoundException(problemDetailsResponse.Detail);
                default:
                    return new InternalServerErrorException(problemDetailsResponse.Detail);
            }
        }

        public static Exception Exception(string exception)
        {
            return new InternalServerErrorException(exception);
        }
    }
}
