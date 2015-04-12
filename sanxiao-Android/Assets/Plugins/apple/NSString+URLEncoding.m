//
//  NSString+URLEncoding.m
//  Unity-iPhone
//
//  Created by 赵 之韵 on 14-1-7.
//
//

#import "NSString+URLEncoding.h"

@implementation NSString (URLEncoding)
-(NSString *)urlEncodeUsingEncoding:(NSStringEncoding)encoding {
	return (NSString *)CFURLCreateStringByAddingPercentEscapes(NULL,
                                                               (CFStringRef)self,
                                                               NULL,
                                                               (CFStringRef)@"!*'\"();:@&=+$,/?%#[]% ",
                                                               CFStringConvertNSStringEncodingToEncoding(encoding));
}
@end
